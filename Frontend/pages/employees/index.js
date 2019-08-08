import React, {Component} from 'react';

export class Employees extends Component {
    componentDidMount() {
        document.title = 'Сотрудники';
    }

    render() {
        return (
            <h1>Сотрудники</h1>
        );
    }
}