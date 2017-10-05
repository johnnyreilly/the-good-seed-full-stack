import { loginPath, LoginPage } from '../login';
import { fxRatesPath, FxRatesPage } from '../features/fxRates';
import { aboutPath, AboutPage } from '../features/about';
import { topicsPath, TopicsPage } from '../features/topics';
import { homePath, HomePage } from '../features/home/index';

export const publicRoutes = [
    { path: loginPath, component: LoginPage, exact: true },
    { path: homePath, component: HomePage, exact: true },
    { path: aboutPath, component: AboutPage, exact: true }
];

export const privateRoutes = [
    { path: topicsPath, component: TopicsPage, exact: false },
    { path: fxRatesPath, component: FxRatesPage, exact: true }
];
