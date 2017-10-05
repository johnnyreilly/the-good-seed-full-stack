import * as React from 'react';
import {
    withRouter,
    Route,
    RouteComponentProps,
    Switch
} from 'react-router-dom';
import { observer, inject } from 'mobx-react';
import DevTools from 'mobx-react-devtools';
import * as classNames from 'classnames';
import { Stores } from '../../stores';
import { SecurityStore } from '../../stores/securityStore';
import { Header } from './header';
import { Menu } from './menu';
import { privateRoutes, publicRoutes } from './routes';
import { NotFound } from '../shared/notFound';
import * as css from '../../styles/styles.scss';
import { config } from '../../config';

interface ILayoutProps extends RouteComponentProps<{}> {
    securityStore: SecurityStore;
}

@withRouter
@inject((stores: Stores) => ({
    securityStore: stores.securityStore
}))
@observer
export class Layout extends React.Component<Partial<ILayoutProps>> {

    renderRoute = ({ path, component, exact }: { path: string; exact?: boolean; component: React.ComponentType<RouteComponentProps<any> | {}> }) => (
        <Route key={path} exact={exact} path={path} component={component} />
    )

    render() {
        const { isLoggedIn } = this.props.securityStore;
        return (
            <div>
                <Header />
                <div className={css.pageBackground}>
                    <Menu />

                    <main className={classNames(css.container, css.section, css.pageContent)} role="main">
                        <Switch>
                            {publicRoutes.map(this.renderRoute)}
                            {isLoggedIn
                                ? privateRoutes.map(this.renderRoute)
                                : null}
                            <Route component={NotFound} />
                        </Switch>
                    </main>
                </div>
                {config.development ? <DevTools /> : null}
            </div>
        );
    }
}
