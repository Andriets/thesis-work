import fastapi

from typing import Type, Callable
from sqlalchemy.ext.asyncio import AsyncSession
from src.api.dependencies.session import get_async_session
from src.db.repositories.base import BaseRepository


def get_repository(
    repo_type: Type[BaseRepository],
) -> Callable[[AsyncSession], BaseRepository]:
    def _get_repo(
        async_session: AsyncSession = fastapi.Depends(get_async_session),
    ) -> BaseRepository:
        return repo_type(async_session=async_session)

    return _get_repo
