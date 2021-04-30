import React from "react";
import styles from "./styles.module.css";

export default function Instruction({ onClose }) {
  return (
    <div className={styles.backdrop} onClick={onClose}>
      <div className={styles.modal}>
        <button className={styles.cross}>╳</button>
        <h2>Инструкция</h2>
        <p>
          Это игра-кликер про самоизоляцию. В игре есть прямоугольнички-домики и
          крыжочки-человечки. Человечки должны сидеть по домам на карантине, но
          они не очень ответственно соблюдают правила и постоянно норовят пойти
          погулять.
        </p>
        <p>
          Это коллаборативная, многопользовательская игра, все пользователи
          вместе стоят на страже здоровья и стараются загонять кружочки по
          домам. Присоединяйся!
        </p>
      </div>
    </div>
  );
}
