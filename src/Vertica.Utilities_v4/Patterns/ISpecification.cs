namespace Vertica.Utilities_v4.Patterns
{
	/* based on JP's implementation with additions from 
	 * http://nichestone.com/blog/08-03-15/simplifying_the_composite_specification_pattern.aspx
	 * http://www.codeinsanity.com/2008/08/implementing-repository-and.html
	 * http://www.mostlyclean.com/post/2008/08/Linq-Expressions-The-Specification-Pattern-and-Repositories-Part-1.aspx
	*/

	public interface ISpecification<T>
	{
		bool IsSatisfiedBy(T item);
		ISpecification<T> And(ISpecification<T> other);
		ISpecification<T> Not();
		ISpecification<T> Or(ISpecification<T> other);
	}
}