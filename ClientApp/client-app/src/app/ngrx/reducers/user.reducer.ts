import { Action, createReducer, on } from '@ngrx/store';
import * as userActions from '../actions/login.action';
import { User } from '../entities/user.entity';

export interface State {
    user: User;
    isLogin: boolean;
}
const userLogged:any = JSON.parse(localStorage.getItem('user'));
export const initialState: State = {
    user: (userLogged && userLogged.user) ? userLogged.user: new User(),
    isLogin: userLogged ? userLogged.isLogin : false
};

const loginReducer = createReducer(
    initialState,
    on(userActions.login, (state, {user}) =>  (
        { 
            user: user, isLogin: true }
        )),
    on(userActions.logout, () => {
        localStorage.removeItem('user');
        return null;
    })
);

export function reducer(state: State | undefined, action: Action): any {
    return loginReducer(state, action);
}