"""Database creation

Revision ID: c2fd452ca350
Revises: 
Create Date: 2023-11-18 13:30:05.499337

"""
from typing import Sequence, Union

from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision: str = 'c2fd452ca350'
down_revision: Union[str, None] = None
branch_labels: Union[str, Sequence[str], None] = None
depends_on: Union[str, Sequence[str], None] = None


def upgrade() -> None:
    # ### commands auto generated by Alembic - please adjust! ###
    op.create_table('prices',
    sa.Column('id', sa.Integer(), nullable=False),
    sa.Column('symbol', sa.String(length=30), nullable=False),
    sa.Column('time', sa.DateTime(), nullable=False),
    sa.Column('price', sa.Double(), nullable=False),
    sa.PrimaryKeyConstraint('id')
    )
    # ### end Alembic commands ###


def downgrade() -> None:
    # ### commands auto generated by Alembic - please adjust! ###
    op.drop_table('prices')
    # ### end Alembic commands ###
