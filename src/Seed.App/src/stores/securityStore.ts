import { observable, action, computed } from 'mobx';
import { asyncAction } from 'mobx-utils';
import { observableAsync } from '../api/async';
import { Api } from '../api/index';
import { ILogin } from '../api/security';

const USER_ID = 'userId';

export class SecurityStore {
  @observable.ref userId: string = null;

  loginResult = observableAsync(this.api.security.createToken);

  @computed get isLoggedIn() {
    return !!this.userId;
  }

  constructor(private api: Api) {
    this.bootstrap();
    window.addEventListener('storage', (event: StorageEvent) => {
      this.userId = event.newValue;
    });
  }

  bootstrap() {
    const userId = window.localStorage.getItem(USER_ID);
    if (userId) {
      this.userId = userId;
    }
  }

  @asyncAction *login(login: ILogin) {
    yield this.loginResult.run(login);
    const { response } = this.loginResult;
    if (response) {
      this.userId = response.userId;
      window.localStorage.setItem(USER_ID, this.userId);
    }
  }

  @action logout() {
    this.loginResult.reset();
    this.userId = null;
    window.localStorage.removeItem(USER_ID);
  }
}