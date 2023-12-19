import pydantic

from sqlalchemy.ext.asyncio import AsyncEngine, AsyncSession, create_async_engine
from sqlalchemy.pool import Pool as SQLAlchemyPool, QueuePool as SQLAlchemyQueuePool
from typing import Union
from src.config.manager import settings


class AsyncDatabase:
    def __init__(self):
        self.postgres_uri: pydantic.PostgresDsn = f"{settings.DB_POSTGRES_SCHEMA}://{settings.DB_POSTGRES_USENRAME}:{settings.DB_POSTGRES_PASSWORD}@{settings.DB_POSTGRES_HOST}:{settings.DB_POSTGRES_PORT}/{settings.DB_POSTGRES_NAME}"

        self.async_engine: AsyncEngine = create_async_engine(
            url=self.set_async_db_uri,
            echo=settings.IS_DB_ECHO_LOG,
            pool_size=settings.DB_POOL_SIZE,
            max_overflow=settings.DB_POOL_OVERFLOW,
            poolclass=SQLAlchemyQueuePool,
        )
        self.async_session: AsyncSession = AsyncSession(bind=self.async_engine)
        self.pool: SQLAlchemyPool = self.async_engine.pool

    @property
    def set_async_db_uri(self) -> Union[str, pydantic.PostgresDsn]:
        """
        Set the synchronous database driver into asynchronous version by utilizing AsyncPG:

            `postgresql://` => `postgresql+asyncpg://`
        """
        return (
            self.postgres_uri.replace("postgresql://", "postgresql+asyncpg://")
            if self.postgres_uri
            else self.postgres_uri
        )


async_db: AsyncDatabase = AsyncDatabase()
