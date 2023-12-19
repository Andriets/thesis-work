import datetime
import requests
import fastapi
import threading
import asyncio

from src.api.dependencies.repository import get_repository
from src.models.schemas.price import PriceInCreate, PriceInResponse
from src.db.repositories.price import PriceRepository
from fastapi.responses import JSONResponse
from src.utilities.schedule import every
from typing import List

router = fastapi.APIRouter(prefix="/price", tags=["price"])


@router.post(
    "/connect",
    name="price:connect_to_binance",
    status_code=fastapi.status.HTTP_200_OK,
)
async def connect_to_binance(
        price_repo: PriceRepository = fastapi.Depends(get_repository(repo_type=PriceRepository)),
) -> JSONResponse:
    async def set_crypto_prices() -> None:
        base_url = "https://api.binance.com/api/v3/ticker/price?symbol="
        currencies = ["BTCUSDT", "ETHUSDT", "SOLUSDT", "ADAUSDT"]

        for curr in currencies:
            url = base_url + curr
            data = requests.get(url)
            data = data.json()

            new_price = PriceInCreate(
                symbol=data['symbol'],
                time=datetime.datetime.now(),
                price=data['price']
            )
            await price_repo.create_price(new_price)

    def between_callback():
        loop = asyncio.get_running_loop()
        task = loop.create_task(every(30, set_crypto_prices))

        try:
            loop.run_until_complete(task)
        except asyncio.CancelledError:
            pass

    threading.Thread(target=between_callback).start()

    return JSONResponse(
        status_code=fastapi.status.HTTP_200_OK,
        content={"message": "Connected"},
    )


@router.post(
    "/",
    name="price:create_price",
    response_model=PriceInResponse,
    status_code=fastapi.status.HTTP_201_CREATED,
)
async def create_price(
        price_create: PriceInCreate,
        price_repo: PriceRepository = fastapi.Depends(get_repository(repo_type=PriceRepository)),
) -> PriceInResponse:
    price_create.time = datetime.datetime.now()
    new_price = await price_repo.create_price(price_create)

    return PriceInResponse(
        id=new_price.id,
        symbol=new_price.symbol,
        time=new_price.time,
        price=new_price.price
    )


@router.get(
    path="/{symbol}",
    name="price:get-prices-by-symbol",
    response_model=List[PriceInResponse],
    status_code=fastapi.status.HTTP_200_OK,
)
async def get_prices_by_symbol(
        symbol: str,
        price_repo: PriceRepository = fastapi.Depends(get_repository(repo_type=PriceRepository)),
) -> List[PriceInResponse]:
    prices = await price_repo.get_prices_by_symbol(symbol, 60)
    response_list: list = list()

    for price in prices:
        res_price = PriceInResponse(
            id=price.id,
            symbol=price.symbol,
            time=price.time,
            price=price.price
        )
        response_list.append(res_price)

    return response_list


@router.get(
    path="",
    name="price:get-all-prices",
    response_model=List[PriceInResponse],
    status_code=fastapi.status.HTTP_200_OK,
)
async def get_prices(
        price_repo: PriceRepository = fastapi.Depends(get_repository(repo_type=PriceRepository)),
) -> List[PriceInResponse]:
    prices = await price_repo.get_prices()
    response_list: list = list()

    for price in prices:
        res_price = PriceInResponse(
            id=price.id,
            symbol=price.symbol,
            time=price.time,
            price=price.price
        )
        response_list.append(res_price)

    return response_list
