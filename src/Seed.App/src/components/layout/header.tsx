import * as React from 'react';
import { withRouter, RouteComponentProps } from 'react-router';
import { inject, observer } from 'mobx-react';
import * as classNames from 'classnames';
import { Link } from 'react-router-dom';
import FaSearch from 'react-icons/lib/fa/search';
import FaSignIn from 'react-icons/lib/fa/sign-in';
import FaSignOut from 'react-icons/lib/fa/sign-out';
import FaCog from 'react-icons/lib/fa/cog';
import { Stores } from '../../stores/index';
import { SecurityStore } from '../../stores/securityStore';
import { loginPath } from '../login';
import * as css from '../../styles/styles.scss';

interface IHeaderProps extends RouteComponentProps<{}> {
    securityStore: SecurityStore;
}

@withRouter
@inject((stores: Stores) => ({
    securityStore: stores.securityStore
}) as IHeaderProps)
@observer
export class Header extends React.Component<Partial<IHeaderProps>> {
    handleLogout = (_e: React.MouseEvent<HTMLAnchorElement>) => {
        this.props.securityStore.logout();
    }

    render() {
        const { pathname } = this.props.location;
        const { isLoggedIn } = this.props.securityStore;
        return (
            <nav className={classNames(css.navbar, css.userNavigation)} role="navigation" aria-label="User navigation">
                <div className={css.container}>
                    <div className={css.navbarBrand}>
                        <Link to="/">
                            LOGO
                        </Link>
                    </div>
                    {isLoggedIn
                        ? (
                            <div className={css.navbarEnd}>
                                <form className={classNames(css.navbarItem, css.userNavSearch)}>
                                    <input type="search" title="search" placeholder="Search" />

                                    <button className={css.buttonReset} type="submit">
                                        <span className={css.icon}>
                                            <FaSearch aria-hidden="true" />
                                        </span>
                                    </button>
                                </form>

                                <a href="#" className={css.navbarItem}>
                                    Settings
                                    {' '}
                                    <span className={css.icon}>
                                        <FaCog aria-hidden="true" />
                                    </span>
                                </a>

                                <a href="#" className={css.navbarItem}>
                                    Notifications
                                    {' '}
                                    <span className={css.notificationCount}>3</span>
                                </a>

                                <Link to="/" onClick={this.handleLogout} className={classNames(css.navbarItem, css.userNavTrigger)}>
                                    Logout
                                    <span className={css.icon}>
                                        <FaSignOut aria-hidden="true" />
                                    </span>
                                </Link>

                                <div className={css.userNavInitials} aria-hidden="true">
                                    JP
                                </div>
                            </div>
                        )

                        : pathname === loginPath
                            ? null
                            : (
                                <div className={css.navbarEnd}>
                                    <Link to={loginPath} className={css.navbarItem}>
                                        Login
                                        {' '}
                                        <span className={css.icon}>
                                            <FaSignIn aria-hidden="true" />
                                        </span>
                                    </Link>
                                </div>
                            )
                    }
                </div>
            </nav >
        );
    }
}
