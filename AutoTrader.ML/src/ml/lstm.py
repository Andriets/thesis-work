from keras.models import load_model
from src.models.schemas.lstm import LSTMResponse
from src.utilities.exceptions.autotrader import AutotraderException
from datetime import datetime as dt
from binance import Client
import joblib
import numpy as np
import pandas as pd

available_symbols = ["BTCUSDT"]
available_invtervals = {
    "5MIN": Client.KLINE_INTERVAL_5MINUTE,
    "30MIN": Client.KLINE_INTERVAL_30MINUTE,
    "60MIN": Client.KLINE_INTERVAL_1HOUR
}
candles_to_predict = 60


def predict(symbol, interval) -> float:
    if interval not in available_invtervals:
        raise AutotraderException("Interval is not available", "")

    model = load_model(f"./ml/models/{symbol}_{interval}.h5")
    scaler = joblib.load(f"./ml/models/{symbol}_{interval}.save")

    client = Client(None, None)
    candles = client.get_klines(symbol=symbol, interval=available_invtervals[interval])

    headers = ["Open Time", "Open", "High", "Low", "Close", "Volume", "Close Time", "QAV", "NAT", "TBBAV", "TBQAV",
               "Ignore"]
    df = pd.DataFrame(candles, columns=headers)
    df['Close'] = df['Close'].str.replace('%', '').astype(np.float64)
    df['Close Time'] = df['Close Time'].apply(lambda timestamp: dt.fromtimestamp(timestamp / 1000))

    df = df[['Close', 'Close Time']]
    df = df.tail(candles_to_predict)

    close = df.filter(["Close"])
    close_array = close.values

    scaled_data = scaler.transform(close_array)

    inputs = np.reshape(scaled_data, (1, candles_to_predict, 1))

    predictions = model.predict(inputs)
    predictions = scaler.inverse_transform(predictions)
    return predictions[0][0]


def get_prediction(symbol):
    if symbol not in available_symbols:
        raise AutotraderException("Symbol is not available", "")

    prediction_5min = predict(symbol, "5MIN")
    prediction_30min = predict(symbol, "30MIN")
    prediction_60min = predict(symbol, "60MIN")

    return LSTMResponse(
        min5=prediction_5min,
        min30=prediction_30min,
        min60=prediction_60min
    )
