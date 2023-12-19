import fastapi

from src.ml.lstm import get_prediction
from src.models.schemas.lstm import LSTMResponse
from src.utilities.exceptions.autotrader import AutotraderException

router = fastapi.APIRouter(prefix="/lstm", tags=["lstm"])


@router.get(
    path="/{symbol}",
    name="lstm:get-prediction",
    response_model=LSTMResponse,
    status_code=fastapi.status.HTTP_200_OK,
)
async def get_test_prediction(symbol: str) -> LSTMResponse:
    response = get_prediction(symbol)

    return response

@router.get(
    path="/hello",
    status_code=fastapi.status.HTTP_200_OK,
)
def hello_world() -> LSTMResponse:
    response = LSTMResponse(
        min5=21000.00,
        min15=21500.00,
        min30=22000.00
    )

    raise AutotraderException("Exception message", "WTF")

    return response
