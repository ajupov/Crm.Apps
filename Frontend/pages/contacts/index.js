import React, {Component} from 'react';

export class Contacts extends Component {
    componentDidMount() {
        document.title = 'Контакты';
    }

    render() {
        return (
            <h1>Контакты</h1>
        );
    }
}