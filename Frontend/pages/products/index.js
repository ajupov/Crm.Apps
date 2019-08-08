import React, {Component} from 'react';

export class Products extends Component {
    componentDidMount() {
        document.title = 'Продукты';
    }

    render() {
        return (
            <h1>Продукты</h1>
        );
    }
}