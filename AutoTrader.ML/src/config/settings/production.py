from src.config.settings.base import BackendBaseSettings
from src.config.settings.environment import Environment
from typing import Optional


class BackendProdSettings(BackendBaseSettings):
    DESCRIPTION: Optional[str] = "Production Environment."
    ENVIRONMENT: Environment = Environment.PRODUCTION
