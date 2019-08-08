import React, {Component} from 'react';

export class Activities extends Component {
    componentDidMount() {
        document.title = 'Активности';
    }

    render() {
        return (
            <h1>Активности</h1>
        );
    }
}