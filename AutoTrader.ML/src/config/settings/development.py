from src.config.settings.base import BackendBaseSettings
from src.config.settings.environment import Environment
from typing import Optional


class BackendDevSettings(BackendBaseSettings):
    DESCRIPTION: Optional[str] = "Development Environment."
    DEBUG: bool = True
    ENVIRONMENT: Environment = Environment.DEVELOPMENT
