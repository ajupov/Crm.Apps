import React, {Component} from 'react';

export class Account extends Component {
    componentDidMount() {
        document.title = 'Аккаунт';
    }

    render() {
        return (
            <h1>Аккаунт</h1>
        );
    }
}