import { FixerApi } from './fixer';
import { SecurityApi } from './security';
import { IConfig } from '../config';

export class Api {
    readonly fixer: FixerApi;
    readonly security: SecurityApi;

    constructor(config: IConfig) { 
        this.fixer = new FixerApi();
        this.security = new SecurityApi(config);
    }
}
