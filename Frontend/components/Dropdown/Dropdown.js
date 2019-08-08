import React, {Component} from 'react';
import PropTypes from 'prop-types';
import Select from 'react-select';
import './Dropdown.less';

class Dropdown extends Component {
    render() {
        return (
            <Select disabled={this.props.disabled}
                    searchable={this.props.searchable}
                    clearable={false}
                    options={this.props.data}
                    value={this.props.value}
                    onChange={this.props.onSelect}
                    placeholder={this.props.placeholder}
                    style={{width: this.props.width}}
            />
        );
    }
}

Dropdown.propTypes = {
    disabled: PropTypes.bool,
    searchable: PropTypes.bool,
    placeholder: PropTypes.string,
    width: PropTypes.number.isRequired,
    data: PropTypes.arrayOf(PropTypes.oneOfType([
        PropTypes.object
    ])).isRequired,
    value: PropTypes.oneOfType([
        PropTypes.number,
        PropTypes.string
    ]),
    onSelect: PropTypes.func.isRequired
};

Dropdown.defaultProps = {
    disabled: false,
    searchable: false,
    placeholder: ''
};

export default Dropdown;