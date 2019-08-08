import React, {Component} from 'react';
import {NavLink} from 'react-router-dom';
import PropTypes from 'prop-types';
import Link from '../Link/Link';

class AddressLink extends Component {
    render() {
        const text = `${this.props.postcode ? this.props.postcode + ', ' : ''}
                        ${this.props.region ? this.props.region + ', ' : ''}
                        ${this.props.province ? this.props.province + ', ' : ''}
                        ${this.props.city ? this.props.city + ', ' : ''}
                        ${this.props.street ? this.props.street + ', ' : ''}
                        ${this.props.house ? this.props.house + ', ' : ''}
                        ${this.props.apartment ? this.props.apartment : ''}`;

        const searchValue = `${this.props.region ? this.props.region + ', ' : ''}
                        ${this.props.province ? this.props.province + ', ' : ''}
                        ${this.props.city ? this.props.city + ', ' : ''}
                        ${this.props.street ? this.props.street + ', ' : ''}
                        ${this.props.house ? this.props.house + ', ' : ''}`;

        const value = `https://yandex.ru/maps?&text=${searchValue}`;

        return (
            <Link className={this.getClassName()} disabled={this.props.disabled} force={true}
                  value={value} target='blank'>{text}</Link>
        );
    }

    getClassName() {
        return this.props.className;
    }
}

AddressLink.propTypes = {
    className: PropTypes.string,
    postcode: PropTypes.string,
    region: PropTypes.string,
    province: PropTypes.string,
    city: PropTypes.string,
    street: PropTypes.string,
    house: PropTypes.string,
    apartment: PropTypes.string,
    disabled: PropTypes.bool
};

AddressLink.defaultProps = {
    className: '',
    disabled: false,
    postcode: '',
    region: '',
    city: '',
    province: '',
    street: '',
    house: '',
    apartment: '',
};

export default AddressLink;