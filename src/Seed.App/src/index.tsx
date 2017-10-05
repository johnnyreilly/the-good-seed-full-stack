import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { BrowserRouter as Router } from 'react-router-dom';
import { Provider } from 'mobx-react';
import { AppContainer } from 'react-hot-loader';
import { App } from './app';
import { Stores } from './stores';
import { Api } from './api';
import { config } from './config';
import './styles/styles.scss';

const api = new Api(config);
const stores = new Stores(api);

const rootEl = document.getElementById('root');
ReactDOM.render(
    <AppContainer>
        <Provider {...stores}>
            <Router>
                <App />
            </Router>
        </Provider>
    </AppContainer>,
    rootEl
);

const anyModule: any = module;

// Hot Module Replacement API
if (anyModule.hot) {
    anyModule.hot.accept('./app', () => {
        const makeNextApp = require('./app').default;
        const nextApp = makeNextApp(['app']);
        ReactDOM.render(
            <AppContainer>
                <Provider {...stores}>
                    <Router>
                        {nextApp.App}
                    </Router>
                </Provider>
            </AppContainer>,
            rootEl
        );
    });
}
