import React, {Component} from 'react';
import PropTypes from 'prop-types';
import styles from './Select.less';
import Input from "../Input/Input";

class Select extends Component {
    constructor() {
        super();

        this.state = {
            value: 0,

        }
    }

    render() {
        return <Input type='text'/>;
    }

    getClassName() {
        return `${styles.chip} ${this.props.className}`;
    }
}

Select.propTypes = {
    className: PropTypes.string
};

Select.defaultProps = {
    className: ''
};

export default Chip;
