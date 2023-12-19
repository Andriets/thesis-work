from datetime import datetime
from pydantic import BaseModel


class PriceInCreate(BaseModel):
    symbol: str
    time: datetime
    price: float


class PriceInResponse(BaseModel):
    id: int
    symbol: str
    time: datetime
    price: float
