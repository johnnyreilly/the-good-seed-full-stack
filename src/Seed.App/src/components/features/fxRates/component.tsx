import * as React from 'react';
import { inject, observer } from 'mobx-react';
import * as classNames from 'classnames';
import { FxRatesStore } from '../../../stores/fxRatesStore';
import { Stores } from '../../../stores';
import { Rates } from './rates';
import * as css from '../../../styles/styles.scss';

export interface IFxRatesProps {
    fxRatesStore: FxRatesStore;
}

interface IState {
    date: string;
}

@inject(
    (stores: Stores) => ({
        fxRatesStore: stores.fxRatesStore
    }) as IFxRatesProps
)
@observer
export class FxRatesPage extends React.Component<Partial<IFxRatesProps>, IState> {
    state = {
        date: '2012-06-15'
    } as IState;

    handleDateChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const { target: { value: date } } = event;
        this.setState(_prevState => ({ date }));
    }

    handleLoadForDateClick = (_event: React.MouseEvent<HTMLButtonElement>) => {
        this.props.fxRatesStore.loadRatesForDate(this.state.date);
    }

    handleLoadLatestClick = (_event: React.MouseEvent<HTMLButtonElement>) => {
        this.props.fxRatesStore.loadRates();
    }

    render() {
        const { date } = this.state;
        const { rates, ratesForDate, lastLoaded } = this.props.fxRatesStore;
        return (
            <div>
                <div className={classNames(css.field)}>
                    <label className={classNames(css.label)}>Date</label>
                    <div className={classNames(css.control)}>
                        <input className={classNames(css.input)} type="text" value={date} onChange={this.handleDateChange} />
                    </div>
                </div>

                <button
                    className={classNames({
                        [css.button]: true,
                        [css.isSuccess]: !ratesForDate.error,
                        [css.isError]: !!ratesForDate.error,
                        [css.isLoading]: ratesForDate.isRequesting
                    })}
                    onClick={this.handleLoadForDateClick}
                >
                    Load 'em up for given date
                </button>

                <button
                    className={classNames({
                        [css.button]: true,
                        [css.isSuccess]: !rates.error,
                        [css.isError]: !!rates.error,
                        [css.isLoading]: rates.isRequesting
                    })}
                    onClick={this.handleLoadLatestClick}
                >
                    Load latest
                </button>

                <p>Last loaded: {lastLoaded === 'ratesForDate' ? 'Rates for date' : lastLoaded === 'rates' ? 'Latest Rates' : null}</p>

                {lastLoaded === 'ratesForDate' ? <Rates {...ratesForDate} /> : null}
                {lastLoaded === 'rates' ? <Rates {...rates} /> : null}
            </div>
        );
    }
}