import * as React from 'react';
import {
    Link,
    withRouter
} from 'react-router-dom';
import { RouteComponentProps } from 'react-router';
import { inject, observer } from 'mobx-react';
import * as classNames from 'classnames';
import { aboutPath } from '../features/about';
import { homePath } from '../features/home';
import { topicsPath } from '../features/topics';
import { fxRatesPath } from '../features/fxRates';
import { Stores } from '../../stores';
import { SecurityStore } from '../../stores/securityStore';
import * as css from '../../styles/styles.scss';

interface IMenuProps extends RouteComponentProps<{}> {
    securityStore: SecurityStore;
}

@withRouter
@inject((stores: Stores) => ({
    securityStore: stores.securityStore
}) as IMenuProps)
@observer
export class Menu extends React.Component<Partial<IMenuProps>> {

    publicLinks = [
        { path: homePath, title: 'Home' },
        { path: aboutPath, title: 'About' },
    ];

    privateLinks = [
        { path: topicsPath, title: 'Topics' },
        { path: fxRatesPath, title: 'FX Rates' }
    ];

    renderLink({ path, title }: { path: string, title: string }) {
        return (
            <Link key={path} to={path} className={classNames(css.navItem, this.isActive(path))}>{title}</Link>
        );
    }

    isActive(to: string) {
        return this.props.location.pathname === to ? css.isActive : '';
    }

    render() {
        return (
            <div className={classNames(css.menu, css.container)}>

                <nav className={classNames(css.navbar, css.siteNavigation)} role="navigation" aria-label="Site navigation">
                    {this.publicLinks.map(link => this.renderLink(link))}
                    {this.props.securityStore.isLoggedIn
                        ? this.privateLinks.map(link => this.renderLink(link))
                        : null}
                </nav>
            </div>
        );
    }
}