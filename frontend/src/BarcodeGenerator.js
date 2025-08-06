import React, { useEffect, useRef } from "react";
import JsBarcode from "jsbarcode";

const BarcodeGenerator = ({ value }) => {
  const barcodeRef = useRef(null);

  useEffect(() => {
    if (barcodeRef.current) {
      JsBarcode(barcodeRef.current, value, {
        format: "CODE128",
        lineColor: "#000",
        width: 2,
        height: 80,
        displayValue: true,
      });
    }
  }, [value]);

  return (
    <div>
      <h2>Generated Barcode</h2>
      <svg ref={barcodeRef}></svg>
    </div>
  );
};

export default BarcodeGenerator;
