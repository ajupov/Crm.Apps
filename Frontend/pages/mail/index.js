import React, {Component} from 'react';

export class Mail extends Component {
    componentDidMount() {
        document.title = 'Почта';
    }

    render() {
        return (
            <h1>Почта</h1>
        );
    }
}