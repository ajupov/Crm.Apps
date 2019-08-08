import React, {Component} from 'react';

export class Restore extends Component {
    componentDidMount() {
        document.title = 'Восстановление пароля';
    }

    render() {
        return (
            <h1>Восстановление пароля</h1>
        );
    }
}