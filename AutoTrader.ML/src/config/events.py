import typing
import asyncio
import requests
import threading
import fastapi
import loguru

from datetime import datetime

from sqlalchemy.ext.asyncio import AsyncSession

from src.api.dependencies.session import get_async_session
from src.db.database import async_db
from src.db.events import dispose_db_connection, initialize_db_connection
from src.api.dependencies.repository import get_repository
from src.db.repositories.price import PriceRepository
from src.models.schemas.price import PriceInCreate
from src.utilities.schedule import every


async def set_crypto_prices() -> None:
    base_url = "https://api.binance.com/api/v3/ticker/price?symbol="
    currencies = ["BTCUSDT", "ETHUSDT", "SOLUSDT", "ADAUSDT"]

    price_repo: PriceRepository = PriceRepository(async_db.async_session)

    for curr in currencies:
        url = base_url + curr
        data = requests.get(url)
        data = data.json()

        new_price = PriceInCreate(
            symbol=data['symbol'],
            time=datetime.now(),
            price=data['price']
        )
        await price_repo.create_price(new_price)


def between_callback():
    loop = asyncio.new_event_loop()
    asyncio.set_event_loop(loop)

    loop.run_until_complete(every(30, set_crypto_prices))
    loop.close()


# def run_async_loop_in_thread():
#     asyncio.run(every(30, set_crypto_prices))


def execute_backend_server_event_handler(backend_app: fastapi.FastAPI) -> typing.Any:
    async def launch_backend_server_events() -> None:
        await initialize_db_connection(backend_app=backend_app)

    # task = asyncio.create_task(every(30, set_crypto_prices))
    # threading.Thread(target=between_callback).start()

    return launch_backend_server_events


def terminate_backend_server_event_handler(backend_app: fastapi.FastAPI) -> typing.Any:
    @loguru.logger.catch
    async def stop_backend_server_events() -> None:
        await dispose_db_connection(backend_app=backend_app)

    return stop_backend_server_events
