import * as mobx from 'mobx';
import { Api } from '../api';
import { CounterStore } from './counterStore';
import { FxRatesStore } from './fxRatesStore';
import { ListerStore } from './listerStore';
import { SecurityStore } from './securityStore';

mobx.useStrict(true); // Use to prevent mysterious issues creeping in https://github.com/mobxjs/mobx/blob/gh-pages/docs/refguide/api.md#usestrict

export class Stores {
    fxRatesStore: FxRatesStore;
    lister1Store: ListerStore;
    lister2Store: ListerStore;
    counterStore: CounterStore;
    securityStore: SecurityStore;

    constructor(api: Api) {
        this.fxRatesStore = new FxRatesStore(api);
        this.lister1Store = new ListerStore();
        this.lister2Store = new ListerStore();
        this.counterStore = new CounterStore();
        this.securityStore = new SecurityStore(api);
    }
}
