import React, {Component} from 'react';
import PropTypes from 'prop-types';
import Button from '../../../components/Button/Button';
import styles from './RightMenuLink.less';

class RightMenuLink extends Component {
    render() {
        return (
            <Button geometry='circle' type='blank' className={styles.button} icon={this.props.icon}
                    onClick={this.props.onClick}/>
        );
    }
}

RightMenuLink.propTypes = {
    onClick: PropTypes.func.isRequired,
    icon: PropTypes.string.isRequired
};

export default RightMenuLink;