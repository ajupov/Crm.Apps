import React, {Component} from 'react'
import {BrowserRouter, Switch, Route} from 'react-router-dom';
import {Path} from '../../enums/Path';
import {post} from '../../helpers/HttpClient';
import Button from '../../components/Button/Button';
import Card from '../../components/Card/Card';
import Checkbox from '../../components/Checkbox/Checkbox';
import Input from '../../components/Input/Input';
import Link from '../../components/Link/Link';
import {AuthenticationProviders} from './providers';
import styles from './index.less';

const urls = {
    signIn: 'Api/V1/Authentication/SignIn'
};

export class Authentication extends Component {
    constructor() {
        super();

        this.state = {
            isLoading: false,
            errors: [],
            login: '',
            password: '',
            isRemember: false
        }
    }

    componentDidMount() {
        document.title = 'Аутентификация';
    }

    render() {
        return (
            <div className={styles.layout}>
                <div className={styles.left}></div>
                <div className={styles.form}>
                    <Card>
                        <h2>Вход</h2>
                        <AuthenticationProviders/>
                        <p className={styles.errors}>{this.state.errors}</p>
                        <Input placeholder='Логин' value={this.state.login}
                               onChange={value => this.setState({login: value})}
                               onKeyDown={value => this.onKeyDownInputs(value)}
                        />
                        <Input placeholder='Пароль' type='password' value={this.state.password}
                               onChange={value => this.setState({password: value})}
                               onKeyDown={value => this.onKeyDownInputs(value)}
                        />
                        <Checkbox label='Запомнить' checked={this.state.isRemember}
                                  onChange={checked => this.setState({isRemember: checked})}/>
                        <div className={styles.signInLayout}>
                            <div className={styles.signInButton}>
                                <Button icon='sign-in' disabled={!this.isSignInAllowed()}
                                        onClick={() => this.onClickSignIn()}>Войти</Button>
                            </div>
                            <div className={styles.links}>
                                <Link value={Path.restore}>Забыли пароль?</Link>
                                <Link value={Path.registration}>Регистрация</Link>
                            </div>
                        </div>
                    </Card>
                </div>
                <div className={styles.right}></div>
            </div>
        )
    }

    signIn() {
        this.setState({
            isLoading: true
        }, () => {
            const data = {
                Key: this.state.login,
                Password: this.state.password,
                IsRemember: this.state.isRemember
            };

            post(urls.signIn, data)
                .then(response => {
                    if (response.isSuccess) {
                        window.location = Path.home;
                    } else {
                        this.setState({
                            isLoading: false,
                            errors: response.errors
                        });
                    }
                })
                .catch(error => {
                    this.setState({
                        isLoading: false,
                        errors: error
                    })
                });
        });
    }

    isSignInAllowed() {
        return this.state.login.length > 0 && this.state.password.length > 0 && !this.state.isLoading;
    }

    onClickSignIn() {
        this.signIn();
    }

    onKeyDownInputs(key) {
        if (key !== 'Enter') {
            return;
        }

        const isSignInAllowed = this.isSignInAllowed();
        if (!isSignInAllowed) {
            return;
        }

        this.signIn();
    }
}