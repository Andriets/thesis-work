from pydantic import BaseModel


class LSTMResponse(BaseModel):
    min5: float
    min30: float
    min60: float
