import React, {Component} from 'react';
import PropTypes from 'prop-types';
import styles from './Table.less';
import Icon from "../Icon/Icon";
import Link from "../Link/Link";

class Table extends Component {
    render() {
        return (
            <div className={styles.tableWrapper}>
                <div className={styles.tableInnerWrapper}>
                    <table className={this.getClassName()}>
                        {this.renderTHead()}
                        {this.renderTBody()}
                    </table>
                </div>
            </div>

        );
    }

    renderTHead() {
        return (

            <thead>
            <tr>
                {this.props.columns.map((column, index) => this.renderTh(column, index))}
                {this.props.editable ? (<th className={styles.actionTh}></th>) : null}
                {this.props.deletable ? (<th className={styles.actionTh}></th>) : null}
            </tr>
            </thead>
        )
    }

    renderTh(column, index) {
        const width = column.width ? `${column.width}px` : 'auto';

        const value = this.props.sortable && column.sortable
            ? (<a>{column.title}</a>)
            : (<span>{column.title}</span>);

        return (
            <th key={index} style={{width: width}}>{value}</th>
        )
    }

    renderTBody() {
        return (
            <tbody>
            {this.props.data.map((row, index) => this.renderTr(row, index))}
            </tbody>
        )
    }

    renderTr(row, rowIndex) {
        return (
            <tr key={rowIndex} onDoubleClick={() => this.props.onClickEdit(row)}>
                {this.props.columns.map((column, index) => this.renderTd(column, index, row, rowIndex))}
                {this.props.editable ? this.renderEditTd(row) : null}
                {this.props.deletable ? this.renderDeleteTd(row) : null}
            </tr>
        );
    }

    renderTd(column, columnIndex, row, rowIndex) {
        const value = row[column.column];
        const formattedValue = column.formatter ? column.formatter(value, row) : value;
        const textAlign = column.textAlign ? column.textAlign : 'left';
        const width = column.width ? `${column.width}px` : 'auto';

        return (
            <td key={`${columnIndex}_${rowIndex}`} style={{width: width, textAlign: textAlign}}>{formattedValue}</td>
        );
    }

    renderEditTd(row) {
        return (
            <td className={styles.icon}><Link onClick={() => this.props.onClickEdit(row)}><Icon name='pencil'/></Link>
            </td>
        );
    }

    renderDeleteTd(row) {
        return (
            <td className={styles.icon}><Link onClick={() => this.props.onClickDelete(row)}><Icon name='remove'/></Link>
            </td>
        );
    }

    getClassName() {
        return `${styles.table} ${this.props.className}`;
    }
}

Table.propTypes = {
    className: PropTypes.string,
    columns: PropTypes.arrayOf(PropTypes.object).isRequired,
    data: PropTypes.arrayOf(PropTypes.object).isRequired,
    loading: PropTypes.bool,
    filterable: PropTypes.bool,
    sortable: PropTypes.bool,
    editable: PropTypes.bool,
    deletable: PropTypes.bool,
    onClickDelete: PropTypes.func,
    onClickEdit: PropTypes.func,
};

Table.defaultProps = {
    className: '',
    loading: false,
    filterable: false,
    sortable: false,
    editable: true,
    deletable: true
};

export default Table;
