const path = require('path');
const TerserPlugin = require('terser-webpack-plugin');

module.exports = {
    entry: './wwwroot/src/index.ts', // Adjust the entry path
    output: {
        filename: 'bundle.js',
        path: path.resolve(__dirname, 'wwwroot/dist') // Adjust the output path
    },
    resolve: {
        extensions: ['.ts', '.js']
    },
    module: {
        rules: [
            {
                test: /\.ts$/,
                use: 'ts-loader',
                exclude: /node_modules/
            }
        ]
    },
    optimization: {
        minimize: true,
        minimizer: [new TerserPlugin()]
    }
};
