import React from "react";
import styles from "./styles.module.css";
import House from "../House";
import Person from "../Person";
import noUiSlider from "nouislider";

export default class Settings extends React.Component {
    constructor() {
        super();
        this.state = {
            countOfDoctor: 0,
        };
    }

    handleSubmit = (evt) => {
        evt.preventDefault();
        const elementList = document.querySelectorAll('.range');
        console.log(elementList[0].value);
    }

    render() {
        const {countOfDoctor} = this.state;

        return (
            <form onSubmit={this.handleSubmit}>
                <div>
                    <label htmlFor="volume">Количество врачей</label>
                    <input type="range" id="countOfDoctor" name="countOfDoctor" className="range"
                           min={countOfDoctor} max="100"/>
                </div>
                <button type='submit'>Применить</button>
            </form>
        );
    }
}