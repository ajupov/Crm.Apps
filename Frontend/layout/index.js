import React, {Component, Fragment} from 'react'
import {NavLink, Route} from 'react-router-dom';
import {HubConnection} from 'signalr-client-react';
import {Path} from '../enums/Path';
import {get, post} from '../helpers/HttpClient';
import {showError} from '../helpers/Notificator';
import {Home} from '../pages/home';
import {Authentication} from './../pages/authentication/index';
import {Registration} from '../pages/registration';
import {Restore} from '../pages/restore';
import {Offer} from '../pages/offer';
import styles from './index.less';
import Main from './components/Main/Main';
import Nav from './components/Nav/Nav';

const urls = {
    getUserContext: 'Api/V1/Account/GetUserContext',
    signOut: 'Api/V1/Authentication/SignOut'
};

export class Layout extends Component {
    constructor() {
        super();

        this.state = {
            isLoading: false,
            isAuthenticated: false,
            hubConnection: null,
            permissions: '',
            timeZone: '',
            userName: '',
            userAvatarUrl: '',
            activitiesCount: 0
        }
    }

    componentDidMount() {
        this.initializeUserContext();

        // let connection = new HubConnection('/main');
        // this.setState({hubConnection: connection},
        //     () => {
        //         this.state.hubConnection
        //             .start()
        //             .then(() => console.log('Connection started!'))
        //             .catch(error => console.log(error));
        //     });
    }

    render() {
        if (this.state.isLoading) {
            return null;
        }

        if (!this.state.isAuthenticated) {
            return (
                <Fragment>
                    <Route path={Path.home} exact component={Home}/>
                    <Route path={Path.authentication} component={Authentication}/>
                    <Route path={Path.registration} component={Registration}/>
                    <Route path={Path.restore} component={Restore}/>
                    <Route path={Path.offer} component={Offer}/>
                </Fragment>
            )
        }

        return (
            <div className={styles.layout}>
                <Nav timeZone={this.state.timeZone} userName={this.state.userName}
                     userAvatarUrl={this.state.userAvatarUrl} activitiesCount={this.state.activitiesCount}/>
                <Main/>
            </div>
        );
    }

    initializeUserContext() {
        this.setState({
            isLoading: true
        }, () => {
            get(urls.getUserContext)
                .then(response => {
                    if (response) {
                        this.setState({
                            isLoading: false,
                            isAuthenticated: true,
                            userName: response.name,
                            userAvatarUrl: response.avatarUrl,
                            permissions: response.permissions,
                            timeZone: response.timeZone
                        });
                    } else {
                        this.setState({isLoading: false});
                    }
                })
                .catch(error => {
                    showError(error);
                });
        });
    }

    onClickLogOut() {
        post(urls.signOut)
            .then(() => {
                window.location = Path.home;

            })
            .catch(error => NotificationHelper.show(error));
    }
}