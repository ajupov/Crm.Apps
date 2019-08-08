import * as React from 'react';
import {get} from '../../../helpers/HttpClient';
import Card from "../../../components/Card/Card";
import Button from "../../../components/Button/Button";
import Label from "../../../components/Label/Label";
import Dropdown from "../../../components/Dropdown/Dropdown";
import './index.less';

export class SettingsCommon extends React.Component {
    constructor() {
        super();

        this.state = {
            countries: [],
            country: 'RU'
        };
    }

    componentDidMount() {
        this.getCountries();
    }

    render() {
        return (
            <Card>
                <h3>Основные настройки</h3>
                {this.renderCountries()}
            </Card>
        );
    };

    renderCountries() {
        if (!this.state.countries || this.state.countries.length === 0 || !this.state.country) {
            return null;
        }

        return (
            <div>
                <Label width='120px'>Часовой пояс</Label>
                <Dropdown value={this.state.country} data={this.state.countries} searchable={true} width={200}
                          onSelect={value => this.onSelectCountry(value)}/>
                <Button onClick={() => {
                    alert()
                }} icon='rocket'>Тест</Button>
            </div>
        );
    }

    getCountries() {
        get('settings/getCountries')
            .then(data => {
                this.setState({
                    countries: data
                });
            });
    }

    onSelectCountry(value) {
        this.setState({
            country: value
        })
    }
}