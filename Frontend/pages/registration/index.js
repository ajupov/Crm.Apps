import React, {Component} from 'react';
import {Path} from '../../enums/Path';
import {post} from '../../helpers/HttpClient';
import Button from '../../components/Button/Button';
import Card from '../../components/Card/Card';
import Input from '../../components/Input/Input';
import Link from '../../components/Link/Link';
import styles from './index.less';

const urls = {
    register: 'Api/V1/Registration/Register'
};

export class Registration extends Component {
    constructor() {
        super();

        this.state = {
            isLoading: false,
            errors: [],
            name: '',
            email: '',
            password: '',
            confirmPassword: ''
        }
    }

    componentDidMount() {
        document.title = 'Регистрация';
    }

    render() {
        return (
            <div className={styles.layout}>
                <div className={styles.left}/>
                <div className={styles.form}>
                    <Card>
                        <h2>Регистрация</h2>
                        <p className={styles.errors}>{this.state.errors}</p>
                        <Input placeholder='Имя' value={this.state.name}
                               onChange={value => this.setState({name: value})}
                               onKeyDown={value => this.onKeyDownInputs(value)}/>
                        <Input placeholder='Email' value={this.state.email}
                               onChange={value => this.setState({email: value})}
                               onKeyDown={value => this.onKeyDownInputs(value)}/>
                        <Input type='password' placeholder='Пароль' value={this.state.password}
                               onChange={value => this.setState({password: value})}
                               onKeyDown={value => this.onKeyDownInputs(value)}/>
                        <Input type='password' placeholder='Повторите пароль' value={this.state.confirmPassword}
                               onChange={value => this.setState({confirmPassword: value})}
                               onKeyDown={value => this.onKeyDownInputs(value)}/>
                        <div className={styles.registerLayout}>
                            <div className={styles.button}>
                                <Button disabled={!this.isRegisterAllowed()}
                                        onClick={() => this.onClickRegister()}>Зарегистрироваться</Button>
                            </div>
                            <div className={styles.links}>
                                <Link value={Path.offer}>Оферта</Link>
                            </div>
                        </div>
                    </Card>
                </div>
                <div className={styles.right}/>
            </div>
        );
    }

    isRegisterAllowed() {
        return this.state.email.length > 0 && this.state.password.length > 0 && this.state.confirmPassword.length > 0 && !this.state.isLoading;
    }

    register() {
        this.setState({
            isLoading: true
        }, () => {
            const data = {
                Name: this.state.name,
                Email: this.state.email,
                Password: this.state.password,
                ConfirmPassword: this.state.confirmPassword
            };

            post(urls.register, data)
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

    onClickRegister() {
        this.register();
    }

    onKeyDownInputs(key) {
        if (key !== 'Enter') {
            return;
        }

        const isRegisterAllowed = this.isRegisterAllowed();
        if (!isRegisterAllowed) {
            return;
        }

        this.register();
    }
}