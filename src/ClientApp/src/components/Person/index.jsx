import React from "react";
import styles from "./styles.module.css";
import {MAX_HEIGHT, MAX_WIDTH} from "../../consts/sizes";
import classNames from "classnames";

export default function Person({person, onClick}) {
    const x = (person.position.x / MAX_WIDTH) * 100;
    const y = (person.position.y / MAX_HEIGHT) * 100;
    const isDoctor = person.profession === 'Doctor';

    return (
        <div
            className={classNames(styles.root, {[styles.sickPerson]: person.isSick, [styles.boredPerson]: person.isBored, [styles.doctor]: isDoctor })}
            style={{left: `${x}%`, top: `${y}%`}}
            onClick={() => onClick(person.id)}
        />
    );
}
