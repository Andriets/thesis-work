import fastapi

from src.api.routes.lstm import router as lstm_router
from src.api.routes.price import router as price_router

router = fastapi.APIRouter()

router.include_router(lstm_router)
router.include_router(price_router)
