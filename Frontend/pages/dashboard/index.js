import React, {Component} from 'react';

export class Dashboard extends Component {
    componentDidMount() {
        document.title = 'Дашбоард';
    }

    render() {
        return (
            <h1>Дашбоард</h1>
        );
    }
}