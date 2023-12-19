import typing

from sqlalchemy import select
from src.db.repositories.base import BaseRepository
from src.models.entities.price import Price
from src.models.schemas.price import PriceInCreate


class PriceRepository(BaseRepository):
    async def create_price(self, create_model: PriceInCreate) -> Price:
        new_price = Price(
            symbol=create_model.symbol,
            time=create_model.time,
            price=create_model.price
        )

        self.async_session.add(instance=new_price)
        await self.async_session.commit()
        await self.async_session.refresh(instance=new_price)

        return new_price

    async def get_prices_by_symbol(self, symbol: str, records_count: int) -> typing.Sequence[Price]:
        stmt = (select(Price)
                .where(Price.symbol == symbol)
                .order_by(Price.id).limit(records_count))

        query = await self.async_session.execute(statement=stmt)

        # if not query:
        #     raise EntityDoesNotExist("")

        return query.scalars().all()  # type: ignore

    async def get_prices(self) -> typing.Sequence[Price]:
        stmt = select(Price)
        query = await self.async_session.execute(statement=stmt)
        res = query.scalars().all()
        return res
