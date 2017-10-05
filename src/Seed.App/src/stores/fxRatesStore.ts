import { observable } from 'mobx';
import { asyncAction } from 'mobx-utils';
import { observableAsync } from '../api/async';
import { Api } from '../api/index';

type LastLoaded = 'rates' | 'ratesForDate';

export class FxRatesStore {
  @observable.ref lastLoaded: LastLoaded = undefined;

  rates = observableAsync(this.api.fixer.getLatestRates);

  ratesForDate = observableAsync(this.api.fixer.getRatesForDate);

  constructor(private api: Api) {}

  @asyncAction *loadRates() {
    yield this.rates.run();
    this.lastLoaded = 'rates';
  }

  @asyncAction *loadRatesForDate(date: string) {
    yield this.ratesForDate.run(date);
    this.lastLoaded = 'ratesForDate';
  }
}