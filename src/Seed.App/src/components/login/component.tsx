import * as React from 'react';
import { Redirect, RouteComponentProps } from 'react-router-dom';
import { observer, inject } from 'mobx-react';
import * as classNames from 'classnames';
import { Stores } from '../../stores';
import { SecurityStore } from '../../stores/securityStore';
import FaUser from 'react-icons/lib/fa/user';
import FaLock from 'react-icons/lib/fa/lock';
import * as css from '../../styles/styles.scss';

export interface ILoginPageProps extends RouteComponentProps<{}> {
    securityStore: SecurityStore;
}

interface IState {
    username: string;
    password: string;
    redirectToReferrer: boolean;
}

@inject((stores: Stores) => ({
    securityStore: stores.securityStore
}))
@observer
export class LoginPage extends React.Component<ILoginPageProps, IState> {

    state = {
        username: '',
        password: ''
    } as IState;

    handleUsernameChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const { target: { value: username } } = event;
        this.setState(_prevState => ({
            username
        }));
    }

    handlePasswordChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const { target: { value: password } } = event;
        this.setState(_prevState => ({
            password
        }));
    }

    handleLogin = async (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault();
        const { securityStore } = this.props;
        const { username, password } = this.state;
        await this.props.securityStore.login({
            username, password
        });
        if (securityStore.userId) {
            this.setState(_prevState => ({ redirectToReferrer: true }));
        }
    }

    render() {
        const { loginResult } = this.props.securityStore;
        const { from } = this.props.location.state || { from: { pathname: '/' } };
        const { redirectToReferrer } = this.state;

        if (redirectToReferrer) {
            return (
                <Redirect to={from} />
            );
        }

        return (
            <div>
                <p>You must log in to view the page at this location: <code>{from.pathname}</code></p>
                <form>
                    <div className={classNames(css.field)}>
                        <p className={classNames(css.control, css.hasIconsRight)}>
                            <input
                                className={classNames(css.input)}
                                type="text"
                                placeholder="username"
                                value={this.state.username}
                                onChange={this.handleUsernameChange}
                            />
                            <span className={classNames(css.icon, css.isSmall, css.isRight)}>
                                <FaUser />
                            </span>
                        </p>
                    </div>
                    <div className={classNames(css.field)}>
                        <p className={classNames(css.control, css.hasIconsRight)}>
                            <input
                                className={classNames(css.input)}
                                type="password"
                                placeholder="Password"
                                value={this.state.password}
                                onChange={this.handlePasswordChange}
                            />
                            <span className={classNames(css.icon, css.isSmall, css.isRight)}>
                                <FaLock />
                            </span>
                        </p>
                    </div>
                    <div className={css.field}>
                        <p className={css.control}>
                            <button
                                className={classNames({
                                    [css.button]: true,
                                    [css.isSuccess]: !loginResult.error,
                                    [css.isError]: !!loginResult.error,
                                    [css.isLoading]: loginResult.isRequesting
                                })}
                                onClick={this.handleLogin}
                            >
                                Login
                            </button>
                        </p>
                    </div >
                </form >
                {loginResult.error ? <div>Login failed with this message: {loginResult.error}</div> : null}
            </div >
        );
    }
}
