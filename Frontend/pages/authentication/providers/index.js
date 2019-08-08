import React, {Component} from 'react'
import {get} from '../../../helpers/HttpClient';
import Icon from '../../../components/Icon/Icon';
import styles from './index.less';

const urls = {
    getProviders: 'Api/V1/Authentication/GetProviders',
    signInExternal: 'Api/V1/Authentication/SignInExternal'
};

export class AuthenticationProviders extends Component {
    constructor() {
        super();

        this.state = {
            data: []
        }
    }

    componentDidMount() {
        this.getProviders();
    }

    render() {
        if (!this.state.data || !this.state.data.length) {
            return null;
        }

        return (
            <div className={styles.layout}>
                {this.state.data.map(provider => this.renderProvider(provider.name))}
            </div>
        )
    }

    renderProvider(name) {
        return (
            <form key={name} className={styles.form} action={urls.signInExternal} method='post'
                  onClick={event => event.currentTarget.submit()}>
                <input name='Provider' value={name} type='hidden'/>
                <div>
                    <Icon name={this.getIconName(name)}/>
                </div>
            </form>
        );
    }

    getProviders() {
        get(urls.getProviders)
            .then(response => {
                this.setState({
                    data: response
                });
            })
    }

    getIconName(name) {
        switch (name) {
            case 'Odnoklassniki':
                return 'odnoklassniki';
                break;
            case 'Google':
                return 'google';
                break;
            case 'Vkontakte':
                return 'vk';
                break;
            default:
                return '';
        }
    }
}