import React, {Component} from 'react';

export class Calendar extends Component {
    componentDidMount() {
        document.title = 'Календарь';
    }

    render() {
        return (
            <h1>Календарь</h1>
        );
    }
}