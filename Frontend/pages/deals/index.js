import React, {Component} from 'react';

export class Deals extends Component {
    componentDidMount() {
        document.title = 'Сделки';
    }

    render() {
        return (
            <h1>Сделки</h1>
        );
    }
}