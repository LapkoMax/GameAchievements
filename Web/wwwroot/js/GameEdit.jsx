class GenreList extends React.Component {
    constructor(props) {
        super(props);
        let genreNames = this.props.gameGenres.split(', ');
        let genres = [];
        this.props.genres.forEach(genre => {
            genreNames.forEach(el => {
                if (el == genre.name) genres.push(genre);
            });
        });
        this.state = { genres: genres };
        this.onDeleteClick = this.onDeleteClick.bind(this);
    }
    onDeleteClick(e) {
        this.state.genres.splice(e.target.value, 1);
        this.setState({ genres: this.state.genres });
    }
    render() {
        return (
            this.state.genres.map(genre => (
                <form name="genreList">
                    <label><p>{genre.name}   </p></label>
                    <button type="button" value={this.state.genres.indexOf(genre)} onClick={this.onDeleteClick} >Delete</button><br />
                </form>
            )));
    }
}

class EditForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = { name: this.props.game.name, description: this.props.game.description, rating: this.props.game.rating, genres: this.props.game.genres };
        this.handleNameChange = this.handleNameChange.bind(this);
        this.handleDescriptionChange = this.handleDescriptionChange.bind(this);
        this.handleRatingChange = this.handleRatingChange.bind(this);
        this.onCancelClick = this.onCancelClick.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }
    handleNameChange(e) {
        this.setState({ name: e.target.value });
    }
    handleDescriptionChange(e) {
        this.setState({ description: e.target.value });
    }
    handleRatingChange(e) {
        this.setState({ rating: e.target.value });
    }
    onCancelClick(e) {
        window.location.href = this.props.url;
    }
    handleSubmit(e) {
        e.preventDefault();
        const name = this.state.name.trim();
        const description = this.state.description.trim();
        let ratingStr = this.state.rating;
        if (this.state.rating.includes(".")) {
            let nums = this.state.rating.trim().split(".");
            ratingStr = (nums[0] + "," + nums[1]);
        }
        const rating = ratingStr;

        const data = new FormData();
        data.append('Id', this.props.game.id);
        data.append('Name', name);
        data.append('Description', description);
        data.append('Rating', rating);

        const xhr = new XMLHttpRequest();
        xhr.open('post', this.props.updateUrl, true);
        xhr.onload = () => this.onCancelClick();
        xhr.send(data);
    }
    render() {
        return (
            <form className="editForm" onSubmit={this.handleSubmit}>
                <label>Game Name:</label><br />
                <input
                    type="text"
                    value={this.state.name}
                    onChange={this.handleNameChange}
                /><br />
                <label>Game Description:</label><br />
                <input
                    type="text"
                    value={this.state.description}
                    onChange={this.handleDescriptionChange}
                /><br />
                <label>Game Rating:</label><br />
                <input
                    type="number"
                    step="0.1"
                    min="0"
                    max="10"
                    value={this.state.rating}
                    onChange={this.handleRatingChange}
                /><br />
                <label>Game Genres:</label><br />
                <GenreList gameGenres={this.state.genres} genres={this.props.genresData} />
                <input type="submit" value="Save Game" />
                <button type="button" name="Cancel" onClick={this.onCancelClick} >Cancel</button>
            </form>
        );
    }
}