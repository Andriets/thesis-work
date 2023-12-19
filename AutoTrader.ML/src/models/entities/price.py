import datetime

from src.models.entities.base import Base

from sqlalchemy import String, DateTime, Double
from sqlalchemy.orm import Mapped
from sqlalchemy.orm import mapped_column


class Price(Base):
    __tablename__ = "prices"

    id: Mapped[int] = mapped_column(primary_key=True, autoincrement="auto")
    symbol: Mapped[str] = mapped_column(String(30), nullable=False)
    time: Mapped[datetime.datetime] = mapped_column(DateTime(), nullable=False)
    price: Mapped[float] = mapped_column(Double, nullable=False)
    