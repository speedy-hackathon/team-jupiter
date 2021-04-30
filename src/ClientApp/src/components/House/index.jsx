import React from "react";
import styles from "./styles.module.css";
import classNames from "classnames";
import {
    HOUSE_HEIGHT,
    HOUSE_WIDTH,
    MAX_HEIGHT,
    MAX_WIDTH,
} from "../../consts/sizes";

const typeOfBuilding = {
    "house": "House",
    "shop": "Shop"
}

export default function House({x, y, building}) {
    const left = (x / MAX_WIDTH) * 100;
    const top = (y / MAX_HEIGHT) * 100;
    const width = (HOUSE_WIDTH / MAX_WIDTH) * 100;
    const height = (HOUSE_HEIGHT / MAX_HEIGHT) * 100;
    return (
        <div
            className={classNames(styles.root, {[styles.shop]: building.buildingType === typeOfBuilding.shop})}
            style={{
                left: `${left}%`,
                top: `${top}%`,
                width: `${width}%`,
                height: `${height}%`,
            }}
        />
    );
}
