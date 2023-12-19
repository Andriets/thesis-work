import time
from loguru import logger


async def every(delay, task):
    next_time = time.time() + delay
    while True:
        time.sleep(max(0, next_time - time.time()))
        try:
            await task()
        except Exception as e:
            # traceback.print_exc()
            logger.exception(f"Problem while executing repetitive task.")
        # skip tasks if we are behind schedule:
        next_time += (time.time() - next_time) // delay * delay + delay
